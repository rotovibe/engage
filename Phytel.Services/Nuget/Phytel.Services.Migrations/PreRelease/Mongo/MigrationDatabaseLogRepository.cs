using AutoMapper;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using Phytel.Services.Dates;
using Phytel.Services.Migrations.Mongo;
using Phytel.Services.Mongo.Repository;
using Phytel.Services.Versions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phytel.Services.Migrations.Mongo
{
    public class MigrationDatabaseLogRepository : IMigrationDatabaseLogRepository
    {
        protected readonly IRepositoryMongo _repositoryMongo;
        protected readonly IMappingEngine _mappingEngine;
        protected readonly IDateTimeProxy _dateTimeProxy;

        public MigrationDatabaseLogRepository(IRepositoryMongo repositoryMongo, IMappingEngine mappingEngine, IDateTimeProxy dateTimeProxy)
        {
            _repositoryMongo = repositoryMongo;
            _dateTimeProxy = dateTimeProxy;
            _mappingEngine = mappingEngine;
        }

        public List<MigrationLog> GetVersionsLessThan(string product, Database db, SemanticVersion version)
        {
            List<MigrationLog> rvalues = new List<MigrationLog>();

            var MigrationLogs = Get(product, db);

            if(MigrationLogs != null && MigrationLogs.Any())
            {
                rvalues = MigrationLogs.Where(m => m.MigrationVersion < version).ToList();
            }

            return rvalues;
        }

        public List<MigrationLog> Get(string product = null, Database db = null)
        {
            List<MigrationLog> rvalues = new List<MigrationLog>();

            var query = _repositoryMongo.Query<MigrationDatabaseLogEntity, ObjectId>(m => m.DeleteFlag == false);
            if(!string.IsNullOrEmpty(product))
            {
                query = query.Where(m => m.Product == product.ToLower());
            }

            if(db != null)
            {
                query = query.Where(m => m.Database.Name == db.Name && m.Database.Server == db.Server && m.Database.Type == db.Type);
            }

            var entities = query.ToList();

            if(entities != null && entities.Any())
            {
                rvalues = _mappingEngine.Map<List<MigrationLog>>(entities);
            }

            return rvalues;
        }

        public List<MigrationLog> GetLatestVersions(string product, List<Database> dbs)
        {
            List<MigrationLog> rvalues = Get(product);

            if (rvalues != null && rvalues.Any())
            {
                rvalues = rvalues
                    .Where(ml => 
                        dbs == null || dbs.Count == 0 || 
                        (
                            dbs.Any(db => 
                                db.Type == ml.Database.Type && 
                                db.Name == ml.Database.Name &&
                                db.Server == ml.Database.Server
                            )
                        )
                    )
                    .GroupBy(ml => new { ml.Database.Type, ml.Database.Name, ml.Database.Server })
                    .Select(mlGroup => mlGroup.OrderByDescending(x => x.MigrationVersion).FirstOrDefault())
                    .ToList();
            }

            return rvalues;
        }

        public MigrationLog GetLatestVersion(string product, Database db)
        {
            MigrationLog rvalue = null;

            var MigrationLogs = Get(product, db);

            if (MigrationLogs != null && MigrationLogs.Any())
            {
                rvalue = MigrationLogs.OrderByDescending(m => m.MigrationVersion).FirstOrDefault();
            }

            return rvalue;
        }

        public MigrationLog Save(string product, SemanticVersion version, Database db, Error error = null)
        {
            MigrationLog rvalue = null;

            DateTime now = _dateTimeProxy.GetCurrentDateTime();

            FindAndModifyArgs args = new FindAndModifyArgs();
            args.Query = Query.And(
                    Query<MigrationDatabaseLogEntity>.EQ(m => m.Product, product.ToLower()),
                    Query<MigrationDatabaseLogEntity>.EQ(m => m.MigrationVersion.Major, version.Major),
                    Query<MigrationDatabaseLogEntity>.EQ(m => m.Database.Name, db.Name),
                    Query<MigrationDatabaseLogEntity>.EQ(m => m.Database.Server, db.Server),
                    Query<MigrationDatabaseLogEntity>.EQ(m => m.Database.Type, db.Type),
                    Query<MigrationDatabaseLogEntity>.EQ(m => m.MigrationVersion.Minor, version.Minor),
                    Query<MigrationDatabaseLogEntity>.EQ(m => m.MigrationVersion.Patch, version.Patch),
                    Query<MigrationDatabaseLogEntity>.EQ(m => m.MigrationVersion.Stage, version.Stage),
                    Query<MigrationDatabaseLogEntity>.EQ(m => m.MigrationVersion.Step, version.Step)
                );
            args.Update = Update<MigrationDatabaseLogEntity>
                .Set(m => m.DeleteFlag, false)
                .Set(m => m.TTLDate, null)
                .Set(m => m.LastUpdatedOn, now)
                .Set(m => m.Error, error)
                .SetOnInsert(m => m.MigrationVersion, version)
                .SetOnInsert(m => m.Database, db)
                .SetOnInsert(m => m.Product, product.ToLower())
                .SetOnInsert(m => m.RecordCreatedOn, now);

            args.Upsert = true;
            args.VersionReturned = FindAndModifyDocumentVersion.Modified;

            MigrationDatabaseLogEntity entity = _repositoryMongo.FindAndModify<MigrationDatabaseLogEntity, ObjectId>(args);

            rvalue = _mappingEngine.Map<MigrationLog>(entity);

            return rvalue;
        }

        public List<MigrationLog> DeleteVersionsLessThan(string product, SemanticVersion version, Database db)
        {
            List<MigrationLog> logsToDelete = GetVersionsLessThan(product, db, version);

            return Delete(logsToDelete);
        }

        public List<MigrationLog> Delete(List<MigrationLog> MigrationLogs)
        {
            List<MigrationLog> rvalues = new List<MigrationLog>();

            foreach(MigrationLog MigrationLog in MigrationLogs)
            {
                MigrationLog rvalue = Delete(MigrationLog);
                if(rvalue != null)
                {
                    rvalues.Add(rvalue);
                }
            }

            return rvalues;
        }

        public MigrationLog Delete(string product, SemanticVersion version, Database db, Error error = null)
        {
            MigrationLog migrationLog = new MigrationLog();
            migrationLog.Product = product;
            migrationLog.Database = db;
            migrationLog.Error = error;
            migrationLog.MigrationVersion = version;
            return Delete(migrationLog);
        }

        public MigrationLog Delete(MigrationLog MigrationLog)
        {
            MigrationLog rvalue = null;

            DateTime now = _dateTimeProxy.GetCurrentDateTime();

            FindAndModifyArgs args = new FindAndModifyArgs();
            args.Query = Query.And(
                    Query<MigrationDatabaseLogEntity>.EQ(m => m.Product, MigrationLog.Product.ToLower()),
                    Query<MigrationDatabaseLogEntity>.EQ(m => m.MigrationVersion.Major, MigrationLog.MigrationVersion.Major),
                    Query<MigrationDatabaseLogEntity>.EQ(m => m.Database.Name, MigrationLog.Database.Name),
                    Query<MigrationDatabaseLogEntity>.EQ(m => m.Database.Server, MigrationLog.Database.Server),
                    Query<MigrationDatabaseLogEntity>.EQ(m => m.Database.Type, MigrationLog.Database.Type),
                    Query<MigrationDatabaseLogEntity>.EQ(m => m.MigrationVersion.Minor, MigrationLog.MigrationVersion.Minor),
                    Query<MigrationDatabaseLogEntity>.EQ(m => m.MigrationVersion.Patch, MigrationLog.MigrationVersion.Patch),
                    Query<MigrationDatabaseLogEntity>.EQ(m => m.MigrationVersion.Stage, MigrationLog.MigrationVersion.Stage),
                    Query<MigrationDatabaseLogEntity>.EQ(m => m.MigrationVersion.Step, MigrationLog.MigrationVersion.Step)
                );
            args.Update = Update<MigrationDatabaseLogEntity>
                .Set(m => m.DeleteFlag, true)
                .Set(m => m.TTLDate, now.AddDays(5))
                .Set(m => m.LastUpdatedOn, now)
                .Set(m => m.Error, MigrationLog.Error)
                .SetOnInsert(m => m.MigrationVersion, MigrationLog.MigrationVersion)
                .SetOnInsert(m => m.Database, MigrationLog.Database)
                .SetOnInsert(m => m.Product, MigrationLog.Product.ToLower())
                .SetOnInsert(m => m.RecordCreatedOn, now);

            args.Upsert = true;
            args.VersionReturned = FindAndModifyDocumentVersion.Modified;

            MigrationDatabaseLogEntity entity = _repositoryMongo.FindAndModify<MigrationDatabaseLogEntity, ObjectId>(args);

            rvalue = _mappingEngine.Map<MigrationLog>(entity);

            return rvalue;
        }
    }
}
