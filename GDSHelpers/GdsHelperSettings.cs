using Microsoft.Extensions.Configuration;

namespace GDSHelpers
{
    internal class GdsHelperSettings
    {
        private static GdsHelperSettings _instance;

        protected GdsHelperSettings(IConfiguration config)
        {
            GdsVersion = config.GetValue<decimal>("GdsHelpers:GdsToolkitVersion");
            Options = new GdsOptions(config);
        }

        public GdsOptions Options { get; }

        public static GdsHelperSettings CreateSingleton(IConfiguration config)
        {
            _instance = _instance ?? new GdsHelperSettings(config);
            return _instance;
        }

        public decimal GdsVersion { get; }

        internal class GdsOptions
        {
            public GdsOptions(IConfiguration config)
            {
                AnchorTagNewWindowText = config.GetValue<string>("GdsHelpers:Options:AnchorTagNewWindowText") ?? "(Set 'opens in ...' text in appsettings.json GdsHelpers:Options:AnchorTagNewWindowText)";
            }

            public string AnchorTagNewWindowText { get; }
        }
    }
}