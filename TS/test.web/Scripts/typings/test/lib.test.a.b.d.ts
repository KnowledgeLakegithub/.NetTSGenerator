/// <reference path="../jquery/jquery.d.ts" />
 declare  module lib {
	
	
	export module test {
			
			
			export module a {
					
					
					export module b {
							
							
/*lib.test.a.b.IShouldNotShowUp*/
export interface IShouldNotShowUp{
  /*properties*/
									ishouldbeignored: string; /*System.String*/

}

								
/*lib.test.a.b.ITestGenericClass`1*/
export interface ITestGenericClass<T>{
  /*properties*/
									data: number[]; /*System.Collections.Generic.IEnumerable`1[System.Int32]*/
  /*methods*/
									GetT?(input:T/*T*/):JQueryPromise<T>;
}

								
/*lib.test.a.b.MYPONO*/
export interface IMYPONO{
  /*properties*/
									Name: string; /*System.String*/

}

							export module Vehicals {
									
									
									export module TwoWheeled {
											
											
											export module MotorCycles {
													
													
/*lib.test.a.b.Vehicals.TwoWheeled.MotorCycles.Harley`1*/
export interface IHarley<T> extends lib.test.a.b.Vehicals.TwoWheeled.MotorCycles.IHog<T>{
  /*properties*/
															MyTrim: any; /*lib.test.a.b.Vehicals.TwoWheeled.MotorCycles.Trim*/
															Type: T; /*T*/

}

														
/*lib.test.a.b.Vehicals.TwoWheeled.MotorCycles.IHog`1*/
export interface IHog<T>{


}

													
													}
													
											}
											
										export module ThreeWheeled {
											
											
/*lib.test.a.b.Vehicals.ThreeWheeled.Spider*/
export interface ISpider{
  /*properties*/
													MyTrim: any; /*lib.test.a.b.Vehicals.TwoWheeled.MotorCycles.Trim*/

}

											
											}
											
									}
									
							}
							
					}
					
			}
			
	}
	