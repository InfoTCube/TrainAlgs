import { TestSolution } from "./testSolution";

export interface TestGroupSolution {
    number: number;
    points: number;
    tests: TestSolution[];
}