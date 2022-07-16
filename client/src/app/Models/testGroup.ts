import { Test } from "./test";

export interface TestGroup {
    number: number;
    points: number;
    tests: Test[];
}