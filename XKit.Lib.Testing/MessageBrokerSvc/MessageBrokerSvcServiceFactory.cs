using System;
using XKit.Lib.Common.Host;
using XKit.Lib.Common.ObjectInstantiation;
using XKit.Lib.Common.Registration;
using XKit.Lib.Common.Services;

namespace XKit.Lib.Testing.TestMessageBrokerSvc {

    public interface IMessageBrokerSvcServiceFactory : IServiceFactory {
		IManagedService Create(
            ILocalEnvironment localEnvironment = null
        );
    }

	public class MessageBrokerSvcServiceFactory : IMessageBrokerSvcServiceFactory
	{
		private static IMessageBrokerSvcServiceFactory factory = new MessageBrokerSvcServiceFactory();

		public static IMessageBrokerSvcServiceFactory Factory => factory;

        IReadOnlyDescriptor IServiceFactory.Descriptor => XKit.Lib.Common.Services.StandardConstants.Managed.StandardServices.MessageBroker.Descriptor;

        // =====================================================================
        // IRegistrySvcServiceFactory
        // =====================================================================

		IManagedService IMessageBrokerSvcServiceFactory.Create(
            ILocalEnvironment localEnvironment
        ) {
            localEnvironment ??= InProcessGlobalObjectRepositoryFactory.CreateSingleton().GetObject<ILocalEnvironment>(); 
            if (localEnvironment == null) { throw new ArgumentNullException(nameof(localEnvironment)); }
            return new MessageBrokerSvcService(localEnvironment);
        } 

        // =====================================================================
        // Static methods
        // =====================================================================

        public static IManagedService Create(
            ILocalEnvironment localEnvironment = null
        ) => Factory.Create(localEnvironment);

        public static void InjectCustomFactory(IMessageBrokerSvcServiceFactory factory) =>
            MessageBrokerSvcServiceFactory.factory = factory; 
	}
}
