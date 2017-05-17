# CleanerSystemMenuItems

This is a simple utility plugin for LaunchBox

Just add *one* of these to your ISystemMenuItemPlugin  and if my plugin is present, boom, it moves your menu item.
```chsarp
public Type CleanerSystemMenuItems_ParentPluginMenuItem => typeof(ClassNameOfParentISystemMenuItemPlugin);
public string CleanerSystemMenuItems_ParentMenuItem => "stocknameOfToolStripMenuItem";
```
Alternate format:
```chsarp
public Type CleanerSystemMenuItems_ParentPluginMenuItem { get{ return typeof(ClassNameOfParentISystemMenuItemPlugin); } }
public string CleanerSystemMenuItems_ParentMenuItem { get{ return "stocknameOfToolStripMenuItem"; } }
```

Possible values for CleanerSystemMenuItems_ParentMenuItem
* installDosGameToolStripMenuItem
* editDefaultToolStripMenuItem
* manageEmulatorsToolStripMenuItem
* managePlatformsToolStripMenuItem
* editAutoHotkeyDosBoxToolStripMenuItem
* editAutoHotkeyScummVmToolStripMenuItem
* editAutoHotkeyWindowsToolStripMenuItem
* selectRandomGameToolStripMenuItem
* downloadToolStripMenuItem
* downloadPlatformThemeVideosToolStripMenuItem
* scanForAddedRomsToolStripMenuItem
* scanForRemovedRomsToolStripMenuItem
* importToolStripMenuItem
* auditToolStripMenuItem
* consolidateRomsToolStripMenuItem
* createMissingArcadePlaylistsToolStripMenuItem
* cleanUpImagesToolStripMenuItem
* refreshImagesToolStripMenuItem
* refreshSelectedImagesToolStripMenuItem
* optionsToolStripMenuItem
