import { Test } from "./test";

export interface TestGroup {
    number: number;
    points: number;
    timeLimit: number;
    tests: Test[];
}