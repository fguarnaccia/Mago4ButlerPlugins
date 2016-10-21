using System.Collections.Generic;
using Microarea.Mago4Butler.Model;
using System;

namespace Microarea.Mago4Butler
{
    public interface IUIMediator
    {
        event EventHandler<ProvisioningEventArgs> ProvisioningNeeded;
        event EventHandler<JobEventArgs> ParametersForInstallNeeded;
        event EventHandler<JobEventArgs> JobNotification;
        bool CanClose { get; }
        bool ShouldShowEmptyUI { get; }
        bool ShouldUserWait { get; }

        void Init();
        void InstallInstance();
        void RemoveInstances(IList<Instance> instances);
        void UpdateInstances(IList<Instance> instances);
    }
}