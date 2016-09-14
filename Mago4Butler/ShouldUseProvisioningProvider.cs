using Microarea.Mago4Butler.Log;
using System;

namespace Microarea.Mago4Butler
{
    public class ShouldUseProvisioningProvider
    {
        readonly LoggerService loggerService;
        readonly PluginService pluginService;
        bool? shouldUseProvisioning = null;

        public ShouldUseProvisioningProvider(PluginService pluginService, LoggerService loggerService)
        {
            this.pluginService = pluginService;
            this.loggerService = loggerService;
        }
        public bool ShouldUseProvisioning
        {
            get
            {
                if (!shouldUseProvisioning.HasValue)
                {
                    shouldUseProvisioning = true;
                    foreach (var plugin in pluginService.Plugins)
                    {
                        if (plugin != null)
                        {
                            try
                            {
                                shouldUseProvisioning &= plugin.ShouldUseProvisioning();
                            }
                            catch (Exception exc)
                            {
                                loggerService.LogError("Error asking plugins if provisioning procedure should be started", exc);
                            }
                        }
                    }
                }

                return shouldUseProvisioning.Value;
            }
        }
    }
}
