require(evolution_era);

imports "Analysis" from "Evolution";

let result = Analysis::open(file = `${@dir}/../demo.dat`);
let creatures = Analysis::creatures(result);

print(creatures, max.print = 6);

write.csv(creatures, file = `${@dir}/creatures.csv`);