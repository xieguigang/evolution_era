require(evolution_era);

imports "Analysis" from "Evolution";

let result = Analysis::open(file = `${@dir}/../demo.dat`);
let characters = Analysis::biology_abundance(result);

print(characters);
