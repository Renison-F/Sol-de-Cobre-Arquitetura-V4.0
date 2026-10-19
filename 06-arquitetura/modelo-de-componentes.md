# Componentes, interfaces e contratos

```mermaid
flowchart TB
 Input[Input Adapter] --> Motor[Character Motor]
 Input --> Abilities[Ability System]
 Abilities --> World[World Interactions]
 World --> Events[Typed Event Bus]
 Events --> Quests[Quest State]
 Quests --> Save[Save Repository]
 Events --> UI[HUD View Model]
 Events --> Audio[Audio / VFX]
```

**ICharacterMotor** calcula locomoção, independente de canvas. **IAbility** expõe pré-condição, execução e interrupção. **IQuestStore** é a única fonte do estado da missão. **ISaveRepository** persiste snapshot com schemaVersion; `IEventBus` publica eventos tipados com ownership explícito.

A UI não destrava missões diretamente; cutscenes observam eventos com skip seguro. Objetos de cena são adaptadores, não núcleo da campanha.
