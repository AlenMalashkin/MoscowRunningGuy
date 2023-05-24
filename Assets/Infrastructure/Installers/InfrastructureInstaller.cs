using VavilichevGD.Utils.Timing;
using Zenject;

public class InfrastructureInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        BindBank();
    }

    private void BindBank()
    {
        Bank bank = new Bank();

        Container
            .Bind<Bank>()
            .FromInstance(bank)
            .AsSingle();
    }
}