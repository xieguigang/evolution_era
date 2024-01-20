require( evolution_era);

let worldModel = Era::world(size = [100,100,3],reproductive.isolation = 0.8,
                          reproduce.rate = 0.5,
                          dna.size = 6,
                          natural.death = 50);
let savefile = file(`${@dir}/demo.dat`);

Era::evolve(worldModel, file = savefile, time = 2000);

close(savefile);