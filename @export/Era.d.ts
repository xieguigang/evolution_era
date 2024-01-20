// export R# package module type define for javascript/typescript language
//
//    imports "Era" from "Evolution";
//
// ref=Evolution.Era@Evolution, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null

/**
 * 
*/
declare namespace Era {
   /**
    * run the simulation
    * 
    * 
     * @param world -
     * @param file the result save file
     * @param time -
     * 
     * + default value Is ``1000``.
     * @param env 
     * + default value Is ``null``.
   */
   function evolve(world: object, file: any, time?: object, env?: object): any;
   /**
    * create a new world
    * 
    * 
     * @param height 
     * + default value Is ``3``.
     * @param reproductive_isolation 
     * + default value Is ``0.9``.
     * @param reproduce_rate 
     * + default value Is ``0.5``.
     * @param dna_size 
     * + default value Is ``6``.
     * @param natural_death 
     * + default value Is ``50``.
     * @param sexual_maturity 
     * + default value Is ``3``.
     * @param predation_diff 
     * + default value Is ``2``.
     * @param water_color 
     * + default value Is ``'#0026ff'``.
     * @param env -
     * 
     * + default value Is ``null``.
   */
   function world(map: object, height?: object, reproductive_isolation?: number, reproduce_rate?: number, dna_size?: object, natural_death?: object, sexual_maturity?: object, predation_diff?: object, water_color?: string, env?: object): object;
}
