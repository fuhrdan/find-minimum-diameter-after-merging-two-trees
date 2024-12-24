//*****************************************************************************
//** 3203. Find Minimum Diameter After Merging Two Trees            leetcode **
//*****************************************************************************

// Define a structure for adjacency lists
typedef struct {
    int* neighbors;
    int size;
    int capacity;
} AdjList;

// Initialize an adjacency list
void initAdjList(AdjList* list, int initialCapacity) {
    list->neighbors = (int*)malloc(initialCapacity * sizeof(int));
    list->size = 0;
    list->capacity = initialCapacity;
}

// Add an edge to the adjacency list
void addEdge(AdjList* list, int neighbor) {
    if (list->size == list->capacity) {
        list->capacity *= 2;
        list->neighbors = (int*)realloc(list->neighbors, list->capacity * sizeof(int));
    }
    list->neighbors[list->size++] = neighbor;
}

// Free the memory used by an adjacency list
void freeAdjList(AdjList* list) {
    free(list->neighbors);
}

int border, diameter;

// DFS to find the tree diameter
void dfs(AdjList* graph, int* graphSize, int v, int p, int d) {
    if (d > diameter) {
        border = v;
        diameter = d;
    }
    for (int i = 0; i < graph[v].size; i++) {
        int u = graph[v].neighbors[i];
        if (u == p) continue;
        dfs(graph, graphSize, u, v, d + 1);
    }
}

// Function to calculate the diameter of a tree
int tree_diameter(AdjList* graph, int* graphSize, int n) {
    diameter = -1;
    dfs(graph, graphSize, 0, -1, 0);
    diameter = -1;
    dfs(graph, graphSize, border, -1, 0);
    return diameter;
}

// Main function to calculate minimum diameter after merging
int minimumDiameterAfterMerge(int** edges1, int edges1Size, int* edges1ColSize, int** edges2, int edges2Size, int* edges2ColSize) {
    int n = edges1Size + 1;
    int m = edges2Size + 1;

    // Create adjacency lists for graph 1
    AdjList* g1 = (AdjList*)malloc(n * sizeof(AdjList));
    for (int i = 0; i < n; i++) {
        initAdjList(&g1[i], 2);
    }
    for (int i = 0; i < edges1Size; ++i) {
        int u = edges1[i][0];
        int v = edges1[i][1];
        addEdge(&g1[u], v);
        addEdge(&g1[v], u);
    }

    // Create adjacency lists for graph 2
    AdjList* g2 = (AdjList*)malloc(m * sizeof(AdjList));
    for (int i = 0; i < m; i++) {
        initAdjList(&g2[i], 2);
    }
    for (int i = 0; i < edges2Size; i++) {
        int u = edges2[i][0];
        int v = edges2[i][1];
        addEdge(&g2[u], v);
        addEdge(&g2[v], u);
    }

    // Calculate diameters
    int d1 = tree_diameter(g1, NULL, n);
    int d2 = tree_diameter(g2, NULL, m);

    if (d1 < d2) {
        int temp = d1;
        d1 = d2;
        d2 = temp;
    }

    // Free memory for adjacency lists
    for (int i = 0; i < n; i++) {
        freeAdjList(&g1[i]);
    }
    free(g1);

    for (int i = 0; i < m; i++) {
        freeAdjList(&g2[i]);
    }
    free(g2);

    return fmax(d1, (int)(ceil(d1 / 2.0) + ceil(d2 / 2.0) + 1));
}