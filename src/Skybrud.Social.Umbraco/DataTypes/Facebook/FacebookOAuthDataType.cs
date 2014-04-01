using System;
using umbraco.cms.businesslogic.datatype;
using umbraco.interfaces;

namespace Skybrud.Social.Umbraco.DataTypes.Facebook {
    
    public class FacebookOAuthDataType : BaseDataType, IDataType {

        private IDataEditor _dataEditor;
        private FacebookOAuthData _data;
        private FacebookOAuthPrevalueEditor _prevalueEditor;

        public override IDataEditor DataEditor {
            get { return _dataEditor ?? (_dataEditor = new FacebookOAuthDataEditor(Data, this)); }
        }

        public override IDataPrevalue PrevalueEditor {
            get { return _prevalueEditor ?? (_prevalueEditor = new FacebookOAuthPrevalueEditor(this)); }
        }

        public override IData Data {
            get { return _data ?? (_data = new FacebookOAuthData(this)); }
        }

        public override Guid Id {
            get { return DataTypeSummary.FacebookOAuth.DataTypeUniqueId; }
        }

        public override string DataTypeName {
            get { return DataTypeSummary.FacebookOAuth.Name; }
        }

        /// <summary>
        /// Gets the options of the underlying prevalue editor.
        /// </summary>
        public FacebookOAuthPrevalueOptions PreValueOptions {
            get { return ((FacebookOAuthPrevalueEditor) PrevalueEditor).GetPreValueOptions(); }
        }

    }

}