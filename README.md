# NearestNeighborSearch_Gridbased
Find the nearest neighbor using the grid based space dividing,

This is designed to find the nearest vertex of A object in B object. In detail: given an index of a vertex in A Mesh, it will searching the nearest index of any vertex in any other meshes.

# Test
  1. Open the scene under Assets\YnPhyx\_Scene\GridBasedSearch.unity
  2. Run.
  
# How it work?
  1. This method is designed based on the Grid world defined by GridCenter(a Vector3, not the origin though), GridNum(how many grids there are in x, y, z dimension), GridSize(x, y, z extent of each grid).
     No extra memory needed for each grid, ie, there is no class Grid. 
  2. Each vertex is then hashed into each grid, and this procedure is really fast.
  3. Searching. If any vertices from another mesh exist in the same grid, put them into the result list, and yet, in order to find the really nearest, we should look for adjacent grids as well.
  
# Limitation
  This method is super fast. Note that I haven't put the MakeGroup procedure offline yet, that will make this even faster.
  But it has its limitations: The Grid world is defined on initialization, and you should define the size of the world carefully to suit your need.
  
