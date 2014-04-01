using System;
using umbraco.cms.businesslogic.datatype;
using umbraco.interfaces;

namespace Skybrud.Social.Umbraco.DataTypes.Instagram {

    public class InstagramOAuthDataType : BaseDataType, IDataType {

        private IDataEditor _dataEditor;
        private InstagramOAuthData _data;
        private InstagramOAuthPrevalueEditor _prevalueEditor;

        public override IDataEditor DataEditor {
            get { return _dataEditor ?? (_dataEditor = new InstagramOAuthDataEditor(Data, this)); }
        }

        public override IDataPrevalue PrevalueEditor {
            get { return _prevalueEditor ?? (_prevalueEditor = new InstagramOAuthPrevalueEditor(this)); }
        }

        public override IData Data {
            get { return _data ?? (_data = new InstagramOAuthData(this)); }
        }

        public override Guid Id {
            get { return DataTypeSummary.InstagramOAuth.DataTypeUniqueId; }
        }

        public override string DataTypeName {
            get { return DataTypeSummary.InstagramOAuth.Name; }
        }

        /// <summary>
        /// Gets the options of the underlying prevalue editor.
        /// </summary>
        public InstagramOAuthPrevalueOptions PreValueOptions {
            get { return ((InstagramOAuthPrevalueEditor)PrevalueEditor).GetPreValueOptions(); }
        }

    }

}