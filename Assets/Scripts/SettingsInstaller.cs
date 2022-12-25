using UnityEngine;
using Zenject;

[CreateAssetMenu(fileName = "SettingsInstaller", menuName = "Installers/SettingsInstaller")]
public class SettingsInstaller : ScriptableObjectInstaller<SettingsInstaller>
{
    public ColorData colorData;
    public TextData textData;
    public override void InstallBindings()
    {
        Container.Bind<ColorData>().FromInstance(colorData).AsSingle();
        Container.Bind<TextData>().FromInstance(textData).AsSingle();
    }
}