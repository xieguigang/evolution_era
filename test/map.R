require(evolution_era);

imports "Analysis" from "Evolution";

setwd(@dir);

let result = Analysis::open(file = "./demo.dat");

for(era in tqdm(0:1000)) {
    bitmap(file = `./dist_maps/${era}.png`) {
        distributionMap(file = result, era = era);
    }
}