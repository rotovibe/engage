using AutoMapper;
using AutoMapper.QueryableExtensions;
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
        protected readonly IRepositoryMongo _repositoryMongo;
        protected readonly IMappingEngine _mappingEngine;
        protected readonly IDateTimeProxy _dateTimeProxy;

        public MigrationRepository(IRepositoryMongo repositoryMongo, IMappingEngine mappingEngine, IDateTimeProxy dateTimeProxy)
        {
            _repositoryMongo = repositoryMongo;
            _dateTimeProxy = dateTimeProxy;
            _mappingEngine = mappingEngine;
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

        public MigrationLog Save(string product, SemanticVersion version)
        {
            return Save(new MigrationLog { Product = product, MigrationVersion = version });
        }

        public MigrationLog Save(MigrationLog migrationLog)
        {
            MigrationLog rvalue = null;

            DateTime now = _dateTimeProxy.GetCurrentDateTime();

            FindAndModifyArgs args = new FindAndModifyArgs();
            args.Query = Query.And(
                    Query<MigrationLogEntity>.EQ(m => m.Product, migrationLog.Product.ToLower()),
                    Query<MigrationLogEntity>.EQ(m => m.MigrationVersion.Major, migrationLog.MigrationVersion.Major),
                    Query<MigrationLogEntity>.EQ(m => m.MigrationVersion.Minor, migrationLog.MigrationVersion.Minor),
                    Query<MigrationLogEntity>.EQ(m => m.MigrationVersion.Patch, migrationLog.MigrationVersion.Patch),
                    Query<MigrationLogEntity>.EQ(m => m.MigrationVersion.Stage, migrationLog.MigrationVersion.Stage),
                    Query<MigrationLogEntity>.EQ(m => m.MigrationVersion.Step, migrationLog.MigrationVersion.Step)
                );
            args.Update = Update<MigrationLogEntity>
                .Set(m => m.DeleteFlag, false)
                .Set(m => m.TTLDate, null)
                .Set(m => m.LastUpdatedOn, now)
                .SetOnInsert(m => m.MigrationVersion, migrationLog.MigrationVersion)
                .SetOnInsert(m => m.Product, migrationLog.Product.ToLower())
                .SetOnInsert(m => m.RecordCreatedOn, now);
            
            args.Upsert = true;
            args.VersionReturned = FindAndModifyDocumentVersion.Modified;

            MigrationLogEntity entity = _repositoryMongo.FindAndModify<MigrationLogEntity, ObjectId>(args);

            rvalue = _mappingEngine.Map<MigrationLog>(entity);

            return rvalue;
        }

        public MigrationLog Delete(string product, SemanticVersion version)
        {
            MigrationLog rvalue = null;

            DateTime now = _dateTimeProxy.GetCurrentDateTime();

            FindAndModifyArgs args = new FindAndModifyArgs();
            args.Query = Query.And(
                    Query<MigrationLogEntity>.EQ(m => m.Product, product.ToLower()),
                    Query<MigrationLogEntity>.EQ(m => m.MigrationVersion.Major, version.Major),
                    Query<MigrationLogEntity>.EQ(m => m.MigrationVersion.Minor, version.Minor),
                    Query<MigrationLogEntity>.EQ(m => m.MigrationVersion.Patch, version.Patch),
                    Query<MigrationLogEntity>.EQ(m => m.MigrationVersion.Stage, version.Stage),
                    Query<MigrationLogEntity>.EQ(m => m.MigrationVersion.Step, version.Step)
                );
            args.Update = Update<MigrationLogEntity>
                .Set(m => m.DeleteFlag, true)
                .Set(m => m.TTLDate, now.AddDays(5))
                .Set(m => m.LastUpdatedOn, now);

            args.VersionReturned = FindAndModifyDocumentVersion.Modified;

            MigrationLogEntity entity = _repositoryMongo.FindAndModify<MigrationLogEntity, ObjectId>(args);

            rvalue = _mappingEngine.Map<MigrationLog>(entity);

            return rvalue;
        }
    }
}