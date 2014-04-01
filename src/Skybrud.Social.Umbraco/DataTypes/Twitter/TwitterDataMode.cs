namespace Skybrud.Social.Umbraco.DataTypes.Twitter {
    
    public enum TwitterDataMode {
    
        /// <summary>
        /// The default data mode only enables OAuth authentication. This ideal for developers that
        /// will handle the rest of the API communication them selves.
        /// </summary>
        Default = 0,

        /// <summary>
        /// When choosing the "Timeline" data mode, the logic behind the property will automatically
        /// fetch tweets from a given users timeline.
        /// </summary>
        Timeline = 1
    
    }

}