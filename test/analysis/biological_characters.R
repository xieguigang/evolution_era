require(evolution_era);

imports "Analysis" from "Evolution";

let result = Analysis::open(file = `${@dir}/../demo.dat`);
let characters = Analysis::biology_abundance(result);

rownames(characters) = 1:nrow(characters);
print(characters, max.print = 6);

write.csv(characters, file = `${@dir}/biological_characters.csv`);