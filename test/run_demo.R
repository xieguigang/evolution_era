require( evolution_era);

let worldModel = Era::world(size = [100,100,3]);
let savefile = file(`${@dir}/demo.dat`);

Era::evolve(worldModel, file = savefile, time = 2000);

close(savefile);