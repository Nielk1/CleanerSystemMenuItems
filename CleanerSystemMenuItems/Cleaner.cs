using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Unbroken.LaunchBox.Plugins;
using Unbroken.LaunchBox.Plugins.Data;

namespace CleanerSystemMenuItems
{
    public class Cleaner : ISystemEventsPlugin
    {
        public void OnEventRaised(string eventType)
        {
            switch (eventType)
            {
                case SystemEventTypes.LaunchBoxStartupCompleted:
                    CleanUpSystemMenu();
                    break;
            }
        }

        /// <summary>
        /// Possible CleanerSystemMenuItems_ParentMenuItem values:
        ///   installDosGameToolStripMenuItem
        ///   editDefaultToolStripMenuItem
        ///   manageEmulatorsToolStripMenuItem
        ///   managePlatformsToolStripMenuItem
        ///   editAutoHotkeyDosBoxToolStripMenuItem
        ///   editAutoHotkeyScummVmToolStripMenuItem
        ///   editAutoHotkeyWindowsToolStripMenuItem
        ///   selectRandomGameToolStripMenuItem
        ///   downloadToolStripMenuItem
        ///   downloadPlatformThemeVideosToolStripMenuItem
        ///   scanForAddedRomsToolStripMenuItem
        ///   scanForRemovedRomsToolStripMenuItem
        ///   importToolStripMenuItem
        ///   auditToolStripMenuItem
        ///   consolidateRomsToolStripMenuItem
        ///   createMissingArcadePlaylistsToolStripMenuItem
        ///   cleanUpImagesToolStripMenuItem
        ///   refreshImagesToolStripMenuItem
        ///   refreshSelectedImagesToolStripMenuItem
        ///   optionsToolStripMenuItem
        /// </summary>
        private void CleanUpSystemMenu()
        {
            Form MainForm = PluginHelper.LaunchBoxMainForm;
            if (MainForm == null) return;
            Control stripX = MainForm?.Controls?["topPanel"]?.Controls?["menuStrip"];
            if (stripX == null) return;
            if (!(stripX is MenuStrip)) return;
            MenuStrip strip = (MenuStrip)(stripX);
            if (strip == null) return;
            ToolStripItem ToolsMenuX = strip?.Items?["toolsToolStripMenuItem"];
            if (ToolsMenuX == null) return;
            if (!(ToolsMenuX is ToolStripMenuItem)) return;
            ToolStripMenuItem ToolsMenu = (ToolStripMenuItem)(ToolsMenuX);
            if (ToolsMenu == null) return;
            if (ToolsMenu?.DropDownItems == null) return;

            Dictionary<Type, Tuple<ISystemMenuItemPlugin, ToolStripMenuItem>> PluginToolStripItems = new Dictionary<Type, Tuple<ISystemMenuItemPlugin, ToolStripMenuItem>>();
            Dictionary<string, ToolStripMenuItem> StockToolStripItems = new Dictionary<string, ToolStripMenuItem>();

            foreach (ToolStripItem tool in ToolsMenu.DropDownItems)
            {
                if (tool.Tag != null && tool.Tag is ISystemMenuItemPlugin)
                {
                    PluginToolStripItems.Add(tool.Tag.GetType(), new Tuple<ISystemMenuItemPlugin, ToolStripMenuItem>((ISystemMenuItemPlugin)(tool.Tag), (ToolStripMenuItem)tool));
                }
                else if (tool is ToolStripMenuItem && !string.IsNullOrWhiteSpace(tool.Name))
                {
                    StockToolStripItems.Add(tool.Name, (ToolStripMenuItem)tool);
                }
            }

            if (PluginToolStripItems.Count > 0)
            {
                foreach (KeyValuePair<Type, Tuple<ISystemMenuItemPlugin, ToolStripMenuItem>> pair in PluginToolStripItems)
                {
                    Type pluginType = pair.Value.Item1.GetType();
                    PropertyInfo property = pluginType.GetProperty("CleanerSystemMenuItems_ParentPluginMenuItem");
                    if (property != null)
                    {
                        try
                        {
                            object RetVal = property.GetMethod.Invoke(pair.Value.Item1, new object[] { });
                            if (RetVal != null && RetVal is Type)
                            {
                                if (PluginToolStripItems.ContainsKey((Type)RetVal))
                                {
                                    ToolsMenu.DropDownItems.Remove(pair.Value.Item2);
                                    PluginToolStripItems[(Type)RetVal].Item2.DropDownItems.Add(pair.Value.Item2);
                                }
                            }
                        }
                        catch { }
                    }
                }

                foreach (KeyValuePair<Type, Tuple<ISystemMenuItemPlugin, ToolStripMenuItem>> pair in PluginToolStripItems)
                {
                    Type pluginType = pair.Value.Item1.GetType();
                    PropertyInfo property = pluginType.GetProperty("CleanerSystemMenuItems_ParentMenuItem");
                    if (property != null)
                    {
                        try
                        {
                            object RetVal = property.GetMethod.Invoke(pair.Value.Item1, new object[] { });
                            if (RetVal != null && RetVal is string)
                            {
                                if (StockToolStripItems.ContainsKey((string)RetVal))
                                {
                                    ToolsMenu.DropDownItems.Remove(pair.Value.Item2);
                                    StockToolStripItems[(string)RetVal].DropDownItems.Add(pair.Value.Item2);
                                }
                            }
                        }
                        catch { }
                    }
                }

                bool AnyPluginRootsLeft = false;
                foreach (ToolStripItem tool in ToolsMenu.DropDownItems)
                {
                    if (tool.Tag != null && tool.Tag is ISystemMenuItemPlugin)
                    {
                        AnyPluginRootsLeft = true;
                        break;
                    }
                }
                if(!AnyPluginRootsLeft)
                {
                    ToolStripItem item = ToolsMenu.DropDownItems[ToolsMenu.DropDownItems.Count - 1];
                    if(item is ToolStripSeparator)
                    {
                        ToolsMenu.DropDownItems.Remove(item);
                    }
                }
            }
        }
    }
}
