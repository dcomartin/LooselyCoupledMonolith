using System;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Hangfire.MediatR
{
    public class MediatorJobActivator : JobActivator
    {
        private readonly IServiceProvider _serviceProvider;

        public MediatorJobActivator(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public override object ActivateJob(Type jobType)
        {
            return jobType == typeof(MediatorJobActivator)
                ? new MediatorHangfireBridge(_serviceProvider.GetRequiredService<IMediator>())
                : _serviceProvider.GetService(jobType);
        }
    }
}