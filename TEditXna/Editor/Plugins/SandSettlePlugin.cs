using TEditXNA.Terraria;
using TEditXna.ViewModel;

namespace TEditXna.Editor.Plugins
{
    public sealed class SandSettlePlugin : BasePlugin
    {
        public SandSettlePlugin(WorldViewModel worldViewModel)
            : base(worldViewModel)
        {
            Name = "Settle Sand";
        }

        public override void Execute()
        {
            if (_wvm.CurrentWorld == null)
                return;

            for (int y = _wvm.CurrentWorld.TilesHigh - 1; y > 0; y--)
            {
                for (int x = 0; x < _wvm.CurrentWorld.TilesWide; x++)
                {
                    var curTile = _wvm.CurrentWorld.Tiles[x, y];
                    bool shouldShift = false;
                    string tileName = World.TileProperties[curTile.Type].Name.ToLower();
                    // Kae 25 Jan 14: Fix so this doesn't match sandstone but
                    // still matches ebonsand, crimsand, etc. Add silt and
                    // slush too.
                    // XXX: Kae 25 Jan 14: Should add a tile property for
                    // "affected by gravity".
                    if (tileName.EndsWith("sand block")) shouldShift = true;
                    if (tileName.StartsWith("silt block")) shouldShift = true;
                    if (tileName.StartsWith("slush block")) shouldShift = true;
                    if (shouldShift)
                    {
                        // check if tile below current tile is empty and move sand to there if it is.
                        int shiftAmmount = 1;
                        while (shiftAmmount + y < _wvm.CurrentWorld.TilesHigh && !_wvm.CurrentWorld.Tiles[x, y + shiftAmmount].IsActive)
                            shiftAmmount++;
                        shiftAmmount--;

                        if (shiftAmmount > 0)
                        {
                            var belowTile = _wvm.CurrentWorld.Tiles[x, y + shiftAmmount];
                            if (!belowTile.IsActive)
                            {
                                _wvm.UndoManager.SaveTile(x, y + shiftAmmount);
                                _wvm.UndoManager.SaveTile(x, y);
                                belowTile.IsActive = true;
                                belowTile.Type = curTile.Type;
                                curTile.IsActive = false;
                                _wvm.UpdateRenderPixel(x, y);
                                _wvm.UpdateRenderPixel(x, y + shiftAmmount);
                            }
                        }
                    }
                }
            }
            _wvm.UndoManager.SaveUndo();
        }
    }
}