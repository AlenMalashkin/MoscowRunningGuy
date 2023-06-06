using UnityEngine;
using Zenject;

public class GameInstaller : MonoInstaller
{
    [SerializeField] private Transform playerSpawnPosition;
    [SerializeField] private LosePanel losePanel;
    [SerializeField] private BoostIndicators boostIndicators;
    [SerializeField] private MusicPlayer musicPlayer;

    public override void InstallBindings()
    {
        BindBoostIndicators();
        BindLosePanel();
        BindMusicPlayer();
        BindPlayer();
    }

    private void BindLosePanel()
    {
        Container
            .Bind<LosePanel>()
            .FromInstance(losePanel)
            .AsSingle();
    }

    private void BindPlayer()
    {
        Player playerPrefab = Resources.Load<Player>("Skins/" + PlayerPrefs.GetString("Skin", "Default"));

        Player player = Container
                            .InstantiatePrefabForComponent<Player>(playerPrefab, playerSpawnPosition.position, Quaternion.identity, null);

        Container
            .Bind<Player>()
            .FromInstance(player)
            .AsSingle();
    }

    private void BindBoostIndicators()
    {
        Container
            .Bind<BoostIndicators>()
            .FromInstance(boostIndicators)
            .AsSingle();
    }

    private void BindMusicPlayer()
    {
        Container
            .Bind<MusicPlayer>()
            .FromInstance(musicPlayer)
            .AsSingle();
    }
}