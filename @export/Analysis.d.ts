// export R# package module type define for javascript/typescript language
//
//    imports "Analysis" from "Evolution";
//
// ref=Evolution.DataFile@Evolution, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null

/**
 * 
*/
declare namespace Analysis {
   /**
   */
   function biology_abundance(file: object): any;
   /**
    * get all creatures from the simulation result
    * 
    * 
     * @param file -
   */
   function creatures(file: object): any;
   /**
   */
   function distributionMap(file: object, era: object): object;
   /**
    * open the simulation result file
    * 
    * 
     * @param file -
     * @param env -
     * 
     * + default value Is ``null``.
   */
   function open(file: any, env?: object): object;
   /**
   */
   function population_size(file: object): any;
}
