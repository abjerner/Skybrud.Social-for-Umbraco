using System;
using System.Linq;
using umbraco.cms.businesslogic.datatype;
using umbraco.cms.businesslogic.datatype.controls;
using umbraco.interfaces;

namespace Skybrud.Social.Umbraco.DataTypes {

    public class DataTypeSummary {

        public static readonly DataTypeSummary FacebookOAuth = new DataTypeSummary {
            UniqueId = new Guid("6b8209eb-7efc-4795-b62e-ee7d84e9e2f1"),
            DataTypeUniqueId = new Guid("95a0c417-7690-4ad8-a7d6-ed853b021e51"),
            Name = "Skybrud.Social - Facebook OAuth",
            Description = "Basic data type for OAuth authentication with Facebook"
        };

        public static readonly DataTypeSummary InstagramOAuth = new DataTypeSummary {
            UniqueId = new Guid("313938bf-3283-4dbb-a5f5-769e3da41a72"),
            DataTypeUniqueId = new Guid("64c76f4a-59f6-4b58-9833-7d2c09fe3b43"),
            Name = "Skybrud.Social - Instagram OAuth",
            Description = "Basic data type for OAuth authentication with Instagram"
        };

        public static readonly DataTypeSummary TwitterOAuth = new DataTypeSummary {
            UniqueId = new Guid("4a5d489f-93be-48f1-b11e-3159185445af"),
            DataTypeUniqueId = new Guid("ee197f73-1e06-4985-8e42-33002813ce8f"),
            Name = "Skybrud.Social - Twitter OAuth",
            Description = "Basic data type for OAuth authentication with Twitter"
        };

        public static readonly DataTypeSummary TwitterTimeline = new DataTypeSummary {
            UniqueId = new Guid("aa349b96-5b45-4ca0-8229-e854411fa368"),
            DataTypeUniqueId = new Guid("2a54242b-cbca-449d-8d92-f42fc5959587"),
            Name = "Skybrud.Social - Twitter Timeline",
            Description = "Data type for embedding a user timeline on Twitter.com"
        };

        public Guid UniqueId { get; private set; }
        public string Name { get; private set; }
        public Guid DataTypeUniqueId { get; private set; }
        public string Description { get; private set; }

        public bool Exists() {
            return DataTypeDefinition.GetAll().Any(x => x.UniqueId == UniqueId);
        }

        public static IDataType GetDataType(Guid guid) {

            // Still using Factory rather than Umbraco.Core.DataTypesResolver
            // due to legacy support. Should we still do so?
            Factory factory = new Factory();

            // Get the data type with the specified GUID
            return factory.GetAll().FirstOrDefault(x => x.Id == guid);

        }

        public static IDataType[] GetDataTypes() {

            // Still using Factory rather than Umbraco.Core.DataTypesResolver
            // due to legacy support. Should we still do so?
            Factory factory = new Factory();

            // Get the data type with the specified GUID
            return factory.GetAll();

        }

        public DataTypeDefinition Create() {

            var dt = GetDataType(DataTypeUniqueId);
            if (dt == null) return null;

            DataTypeDefinition dtd = DataTypeDefinition.MakeNew(umbraco.BusinessLogic.User.GetCurrent(), Name, UniqueId);
            dtd.DataType = dt;
            dtd.Save();

            return dtd;

        }

        public void Delete() {
            DataTypeDefinition dtd = DataTypeDefinition.GetAll().FirstOrDefault(x => x.UniqueId == UniqueId);
            if (dtd != null) dtd.delete();
        }

        public static DataTypeSummary[] GetAll() {
            return new[] {
                FacebookOAuth,
                InstagramOAuth
            };
        }

        public static DataTypeSummary GetById(Guid guid) {
            return GetAll().FirstOrDefault(x => x.UniqueId == guid);
        }

        public static DataTypeSummary GetById(string guid) {
            return GetAll().FirstOrDefault(x => x.UniqueId == new Guid(guid));
        }

    }

}