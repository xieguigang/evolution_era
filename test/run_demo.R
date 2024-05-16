require( evolution_era);


let worldModel = Era::world(defaultWorldMap(),
    reproductive.isolation = 0.85,
                          reproduce.rate = 0.25,
                          dna.size = 8,
                          natural.death = 45);
let savefile = file(`${@dir}/demo.dat`, truncate = TRUE);

Era::evolve(worldModel, file = savefile, time = 8000);

close(savefile);