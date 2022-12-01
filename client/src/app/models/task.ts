import { TestGroup } from "./testGroup";

export interface AlgTask {
    name: string;
    nameTag: string;
    content: string;
    timeLimit: number;
    memoryLimit: number;
    authorUsername: string;
    exampleTestGroup: TestGroup;
}
