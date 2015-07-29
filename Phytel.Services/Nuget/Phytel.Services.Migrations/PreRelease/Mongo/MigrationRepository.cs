using AutoMapper;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using Phytel.Services.Dates;
using Phytel.Services.Mongo.Repository;
using Phytel.Services.Versions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Phytel.Services.Migrations.Mongo
{
    public class MigrationRepository : IMigrationRepository
    {
        protected readonly IDateTimeProxy _dateTimeProxy;
        protected readonly IMappingEngine _mappingEngine;
        protected readonly IRepositoryMongo _repositoryMongo;

        public MigrationRepository(IRepositoryMongo repositoryMongo, IMappingEngine mappingEngine, IDateTimeProxy dateTimeProxy)
        {
            _repositoryMongo = repositoryMongo;
            _dateTimeProxy = dateTimeProxy;
            _mappingEngine = mappingEngine;
        }

        public MigrationLog Delete(string product, SemanticVersion version, Error err = null)
        {
            MigrationLog rvalue = null;

            DateTime now = _dateTimeProxy.GetCurrentDateTime();

            FindAndModifyArgs args = new FindAndModifyArgs();
            args.Query = Query.And(
                    Query<MigrationDatabaseLogEntity>.EQ(m => m.Product, product.ToLower()),
                    Query<MigrationDatabaseLogEntity>.EQ(m => m.MigrationVersion.Major, version.Major),
                    Query<MigrationDatabaseLogEntity>.EQ(m => m.MigrationVersion.Minor, version.Minor),
                    Query<MigrationDatabaseLogEntity>.EQ(m => m.MigrationVersion.Patch, version.Patch),
                    Query<MigrationDatabaseLogEntity>.EQ(m => m.MigrationVersion.Stage, version.Stage),
                    Query<MigrationDatabaseLogEntity>.EQ(m => m.MigrationVersion.Step, version.Step)
                );
            args.Update = Update<MigrationLogEntity>
                .Set(m => m.DeleteFlag, true)
                .Set(m => m.TTLDate, now.AddDays(5))
                .Set(m => m.LastUpdatedOn, now)
                .Set(m => m.Error, err)
                .SetOnInsert(m => m.MigrationVersion, version)
                .SetOnInsert(m => m.Product, product.ToLower())
                .SetOnInsert(m => m.RecordCreatedOn, now);

            args.Upsert = true;
            args.VersionReturned = FindAndModifyDocumentVersion.Modified;

            MigrationLogEntity entity = _repositoryMongo.FindAndModify<MigrationLogEntity, ObjectId>(args);

            rvalue = _mappingEngine.Map<MigrationLog>(entity);

            return rvalue;
        }

        public List<MigrationLog> Get(string product = null)
        {
            List<MigrationLog> rvalues = new List<MigrationLog>();

            IQueryable<MigrationLogEntity> query = _repositoryMongo.Query<MigrationLogEntity, ObjectId>(m => m.DeleteFlag == false);
            if (!string.IsNullOrEmpty(product))
            {
                query = query.Where(m => m.Product == product.ToLower());
            }

            List<MigrationLogEntity> entities = query.ToList();

            rvalues = _mappingEngine.Map<List<MigrationLog>>(entities);

            return rvalues;
        }

        public MigrationLog GetLatestVersion(string product)
        {
            MigrationLog rvalue = null;

            var migrationLogs = Get(product);

            if (migrationLogs != null && migrationLogs.Any())
            {
                rvalue = migrationLogs.OrderByDescending(m => m.MigrationVersion).FirstOrDefault();
            }

            return rvalue;
        }

        public MigrationLog Save(string product, SemanticVersion version, Error err = null)
        {
            MigrationLog rvalue = null;

            DateTime now = _dateTimeProxy.GetCurrentDateTime();

            FindAndModifyArgs args = new FindAndModifyArgs();
            args.Query = Query.And(
                    Query<MigrationDatabaseLogEntity>.EQ(m => m.Product, product.ToLower()),
                    Query<MigrationDatabaseLogEntity>.EQ(m => m.MigrationVersion.Major, version.Major),
                    Query<MigrationDatabaseLogEntity>.EQ(m => m.MigrationVersion.Minor, version.Minor),
                    Query<MigrationDatabaseLogEntity>.EQ(m => m.MigrationVersion.Patch, version.Patch),
                    Query<MigrationDatabaseLogEntity>.EQ(m => m.MigrationVersion.Stage, version.Stage),
                    Query<MigrationDatabaseLogEntity>.EQ(m => m.MigrationVersion.Step, version.Step)
                );
            args.Update = Update<MigrationLogEntity>
                .Set(m => m.DeleteFlag, false)
                .Set(m => m.TTLDate, null)
                .Set(m => m.LastUpdatedOn, now)
                .Set(m => m.Error, err)
                .SetOnInsert(m => m.MigrationVersion, version)
                .SetOnInsert(m => m.Product, product.ToLower())
                .SetOnInsert(m => m.RecordCreatedOn, now);

            args.Upsert = true;
            args.VersionReturned = FindAndModifyDocumentVersion.Modified;

            MigrationLogEntity entity = _repositoryMongo.FindAndModify<MigrationLogEntity, ObjectId>(args);

            rvalue = _mappingEngine.Map<MigrationLog>(entity);

            return rvalue;
        }
    }
}