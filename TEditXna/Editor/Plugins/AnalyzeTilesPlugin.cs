using System.ComponentModel;
using System.Windows;
using TEditXNA.Terraria;
using TEditXna.ViewModel;
using System.Collections.Generic;
using System.Collections;

namespace TEditXna.Editor.Plugins
{
    public class AnalyzeTilesPlugin : BasePlugin
    {
        public AnalyzeTilesPlugin(WorldViewModel worldViewModel)
            : base(worldViewModel)
        {
            Name = "Analyze Tiles";
        }


        public override void Execute()
        {
            SortedDictionary<int, int> counts = new SortedDictionary<int, int>();
            SortedDictionary<string, int> results = new SortedDictionary<string, int>();
            if (_wvm.CurrentWorld == null)
                return;

            int x0 = 0;
            int y0 = 0;
            int xN = _wvm.CurrentWorld.TilesWide;
            int yN = _wvm.CurrentWorld.TilesHigh;
            if (_wvm.Selection.IsActive) {
                x0 = _wvm.Selection.SelectionArea.Left;
                y0 = _wvm.Selection.SelectionArea.Top;
                xN = _wvm.Selection.SelectionArea.Right;
                yN = _wvm.Selection.SelectionArea.Bottom;
            }
            for (int x = x0; x < xN; x++) {
                for (int y = y0; y < yN; y++) {
                    Tile curTile = _wvm.CurrentWorld.Tiles[x, y];
                    int count = 0;
                    if (counts.ContainsKey(curTile.Type)) {
                        count = counts[curTile.Type];
                    }
                    counts[curTile.Type] = count + 1;
                }
            }
            
            string resultText = "";
            IDictionaryEnumerator e = counts.GetEnumerator();
            while (e.MoveNext()) {
                int t = (int)e.Key;
                string tileName = (t != 0 ? World.TileProperties[t].Name : "[empty]");
                if (tileName.Length == 0) tileName = "[unclassified]";
                string key = string.Format("{0} ({1})", tileName, e.Key);
                results[key] = (int)e.Value;
            }

            e = results.GetEnumerator();
            while (e.MoveNext()) {
                resultText += string.Format("{0,-25} {1}\r\n", e.Key, e.Value);
            }

            AnalyzeTilesPluginView view = new AnalyzeTilesPluginView(resultText);
            view.Owner = App.Current.MainWindow;
            view.DataContext = _wvm;
            view.Show();
        }
    }
}

