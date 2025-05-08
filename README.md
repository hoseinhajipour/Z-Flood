# Z-Flood - Zombie Survival Game

![Unity Version](https://img.shields.io/badge/Unity-2022.3+-black.svg?logo=unity)
![Platforms](https://img.shields.io/badge/Platforms-Windows%20%7C%20Android-blue.svg)

A top-down zombie survival shooter with RPG elements and dynamic combat systems.

## Features

### Core Gameplay
- Health system with pickup items (`Health.cs`, `HealthPackPickup.cs`)
- Weapon management with upgradeable firearms (`WeaponManager.cs`, `shotgun.asset`)
- Auto-shooting mechanics and bullet physics (`AutoShooter.cs`, `Bullet.cs`)
- Progressive reward system for skill upgrades (`RewardSystem.cs`)

### AI & Combat
- Zombie detection range visualization (`DetectionRangeVisualizer.cs`)
- Camera follow system for player tracking (`CameraFollow.cs`)

### UI/UX
- Game state UI with health bars (`GameUI.cs`, `healthBarPrefab.prefab`)
- Shop interface and upgrade system (`ShopUI.cs`, `SkillUpgradeUI.cs`)
- Tapsell Plus integration for ads/rewards (`TapSellInitializer.cs`)

## Installation

1. **Requirements**
   - Unity 2022.3+
   - Android Build Support (for mobile deployment)
   - TextMesh Pro Package

2. **Setup**
```bash
# Clone repository
git clone https://github.com/your-username/z-flood.git
```

3. **Dependencies**
   - Import TextMesh Pro via Package Manager
   - Add TapsellPlusSDK to Assets/ folder
   - Configure Android settings in Player Preferences

## Project Structure

```
Assets/
├── Scripts/          # Core game logic
│   ├── Player/       # Player controllers and mechanics
│   ├── UI/           # User interface systems
│   └── Weapons/      # Weapon configurations and logic
├── TextMesh Pro/     # Text rendering system
└── TapsellPlusSDK/   # Ad integration toolkit
```

## License
[//]: # (Add license information here)