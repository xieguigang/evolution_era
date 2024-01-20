require( evolution_era);

let worldModel = Era::world(size = [200,200,3],reproductive.isolation = 0.8,
                          reproduce.rate = 0.45,
                          dna.size = 6,
                          natural.death = 30);
let savefile = file(`${@dir}/demo.dat`);

Era::evolve(worldModel, file = savefile, time = 1000);

close(savefile);