namespace YupMauiBlazor.Utilities
{
    public class Settings
    {

        /// <summary>
        /// Howmany items are requested in each ListBox scroll down (see Video.cs and SearchControlView.axaml.cs)
        /// </summary>
        public static int MAX_ITEMS_CHUNK = 10;

        /// <summary>
        /// Image to be used when it's not possible to get a media thumbnail
        /// </summary>
        public const string YupImageNotAvailable = @"./Assets/covernotfound.jpg";
    }
}
