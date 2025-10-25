
public static class Sorting
{
    public static void QuickSortRooms(Room[] rooms, float[] distances, int low, int high)
    {
        if (rooms == null || distances == null) return;
        if (rooms.Length != distances.Length) return;
        if (low >= high) return;


        int pivotIndex = Partition(rooms, distances, low, high);

        // Sort left / right
        QuickSortRooms(rooms, distances, low, pivotIndex - 1);
        QuickSortRooms(rooms, distances, pivotIndex + 1, high);
    }

    // Lomuto partition scheme
    private static int Partition(Room[] rooms, float[] distances, int low, int high)
    {
        float pivot = distances[high];
        int i = low - 1;

        for (int j = low; j < high; j++)
        {
            if (distances[j] <= pivot)
            {
                i++;
                Swap(rooms, distances, i, j);
            }
        }

        Swap(rooms, distances, i + 1, high);
        return i + 1;
    }

    private static void Swap(Room[] rooms, float[] distances, int a, int b)
    {
        // swap distances
        float tempD = distances[a];
        distances[a] = distances[b];
        distances[b] = tempD;

        // swap rooms
        Room tempR = rooms[a];
        rooms[a] = rooms[b];
        rooms[b] = tempR;
    }
}
