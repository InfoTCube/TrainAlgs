import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { CodeViewerComponent } from './code-viewer/code-viewer.component';
import 'prismjs/components/prism-c';
import 'prismjs/components/prism-cpp';
import 'prismjs/components/prism-python';

@NgModule({
  declarations: [],
  imports: [
    CommonModule
  ],
  exports: []
})
export class SharedModule { }
