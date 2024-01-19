require( evolution_era);

let worldModel = Era::world();
let savefile = file(`${@dir}/demo.dat`);

Era::evolve(worldModel, file = savefile);

close(savefile);