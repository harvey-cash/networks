using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Map {

    public const int MAP_X = 100, MAP_Z = 100;
    public const int DATA_LAYERS = 1;

    // Layer zero is "object taking up space"
    public const int SPACE_LAYER = 0, SPACE_FULL = 1, SPACE_EMPTY = 0;

    // Assigned by any and everything
    public static int[,,] grid = new int[MAP_X, MAP_Z, DATA_LAYERS];

    // Adjust for negative coords
    public static Vector3 WorldCoordsToCell(Vector3 pos) {
        Vector3 snap = SnapToGrid(pos);
        return new Vector3(snap.x + MAP_X / 2, 0, snap.z + MAP_Z / 2);
    }

    // Compare to grid size
    public static bool WithinBounds(Vector3 cell, int layer) {
        // Within bounds
        if (cell.x < 0 || cell.x > MAP_X || cell.z < 0 || cell.z > MAP_Z) {
            return false;
        }
        // Actual layer
        if (layer < 0 || layer > DATA_LAYERS - 1) {
            return false;
        }
        // Must be okay!
        else {
            return true;
        }
    }

    /** ~~~~~~~ GETTING / SETTING CELLS ~~~~~~~ **/

    // Arrays can only be positive, but the world has negative co-ords!
    // Shift by half array size
    // Return true if successful (within bounds)
    public static bool SetWorldCell(Vector3 worldPos, int layer, int value) {
        Vector3 cell = WorldCoordsToCell(worldPos);
        if (!WithinBounds(cell, layer)) {
            return false;
        }        
        else {
            grid[(int)cell.x, (int)cell.z, layer] = value;
            return true;
        }
    }

    // Do multiple cells!
    public static bool SetWorldCells(Vector3 worldPos, Vector3[] offsets, int layer, int value) {
        // Check all the spaces around the center, if any are out of bounds then that's it
        for (int i = 0; i < offsets.Length; i++) {
            Vector3 cell = WorldCoordsToCell(worldPos + offsets[i]);
            if (!WithinBounds(cell, layer)) {
                return false;
            }
        }

        // If we get this far, we're all good to set values
        for (int i = 0; i < offsets.Length; i++) {
            SetWorldCell(worldPos + offsets[i], layer, value);
        }
        return true;
    }

    // Access the grid cell at the given world coords
    // Throws error (for now) if out of bounds
    public static int GetWorldCell(Vector3 worldPos, int layer) {
        Vector3 cell = WorldCoordsToCell(worldPos);
        if (WithinBounds(cell, layer)) {
            return grid[(int)(cell.x), (int)cell.z, layer];
        }
        else {
            throw new System.Exception("Out of bounds!");
            //return SPACE_FULL;
        }        
    }

    // Access multiple at once!
    public static int[] GetWorldCells(Vector3 worldPos, Vector3[] offsets, int layer) {
        int[] cells = new int[offsets.Length];

        for (int i = 0; i < offsets.Length; i++) {
            cells[i] = GetWorldCell(worldPos + offsets[i], layer);
        }

        return cells;
    }

    // Simply rounds to unit spacing
    public static Vector3 SnapToGrid(Vector3 vector) {
        return Maths.VectorRound(vector);
    }

}
