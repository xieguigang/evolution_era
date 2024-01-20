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
     * @param size -
     * 
     * + default value Is ``[10,10,3]``.
     * @param reproductive_isolation 
     * + default value Is ``0.9``.
     * @param reproduce_rate 
     * + default value Is ``0.5``.
     * @param dna_size 
     * + default value Is ``5``.
     * @param natural_death 
     * + default value Is ``100``.
     * @param env -
     * 
     * + default value Is ``null``.
   */
   function world(size?: any, reproductive_isolation?: number, reproduce_rate?: number, dna_size?: object, natural_death?: object, env?: object): object;
}
