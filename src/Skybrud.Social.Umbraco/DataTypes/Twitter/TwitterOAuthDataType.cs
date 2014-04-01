using System;
using umbraco.cms.businesslogic.datatype;
using umbraco.interfaces;

namespace Skybrud.Social.Umbraco.DataTypes.Twitter {

    public class TwitterOAuthDataType : BaseDataType, IDataType {

        private IDataEditor _dataEditor;
        private TwitterOAuthData _data;
        private TwitterOAuthPrevalueEditor _prevalueEditor;

        public override IDataEditor DataEditor {
            get { return _dataEditor ?? (_dataEditor = new TwitterOAuthDataEditor(Data, this)); }
        }

        public override IDataPrevalue PrevalueEditor {
            get { return _prevalueEditor ?? (_prevalueEditor = new TwitterOAuthPrevalueEditor(this)); }
        }

        public override IData Data {
            get { return _data ?? (_data = new TwitterOAuthData(this)); }
        }

        public override Guid Id {
            get { return DataTypeSummary.TwitterOAuth.DataTypeUniqueId; }
        }

        public override string DataTypeName {
            get { return DataTypeSummary.TwitterOAuth.Name; }
        }

        /// <summary>
        /// Gets the options of the underlying prevalue editor.
        /// </summary>
        public TwitterOAuthPrevalueOptions PreValueOptions {
            get { return ((TwitterOAuthPrevalueEditor) PrevalueEditor).GetPreValueOptions(); }
        }

    }

}