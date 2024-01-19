require(evolution_era);

imports "Analysis" from "Evolution";

let result = Analysis::open(file = `${@dir}/../demo.dat`);
let size = result |> population_size();

print(size);

bitmap(file = `${@dir}/population_size.png`) {
    plot(size, grid.fill = "white");
}