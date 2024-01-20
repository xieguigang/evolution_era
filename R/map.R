#' get default map image for create a new world
#' 
const defaultWorldMap = function() {
    return(readImage(system.file("data/world_map.bmp", package = "evolution_era")));
}