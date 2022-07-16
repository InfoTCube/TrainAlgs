import { TestSolution } from "./testSolution";

export interface TestGroupSolution {
    number: number;
    points: number;
    maxPoints: number;
    tests: TestSolution[];
}