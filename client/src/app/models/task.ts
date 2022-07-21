import { TestGroup } from "./testGroup";

export interface AlgTask {
    name: string;
    nameTag: string;
    content: string;
    authorUsername: string;
    exampleTestGroup: TestGroup;
}
