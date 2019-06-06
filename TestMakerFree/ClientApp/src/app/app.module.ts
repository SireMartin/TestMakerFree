import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { RouterModule } from '@angular/router';

import { AppComponent } from './app.component';
import { NavMenuComponent } from './nav-menu/nav-menu.component';
import { HomeComponent } from './home/home.component';
import { QuizListComponent } from './quiz/quiz-list.component';
import { QuizComponent } from './quiz/quiz.component';
import { LoginComponent } from './login/login.component';
import { AboutComponent } from './about/about.component';
import { PageNotFoundComponent } from './pagenotfound/filenotfound.component';
import { QuizEditComponent } from './quiz/quiz-edit.component';
import { QuestionListComponent } from './question/question-list.component';
import { QuestionEditComponent } from './question/question-edit.component';
import { AnswerListComponent } from './answer/answer-list.component';
import { AnswerEditComponent } from './answer/answer-edit.component';
import { ResultListComponent } from './result/result-list.component';
import { ResultEditComponent } from './result/result-edit.component';
 
@NgModule({
  declarations: [
    AppComponent,
    NavMenuComponent,
    HomeComponent,
    QuizListComponent,
    QuizComponent,
    LoginComponent,
    AboutComponent,
    PageNotFoundComponent,
    QuizEditComponent,
    QuestionListComponent,
    //deze eens vergeten toe te voegen: browser F12 = Uncaught Error: Component QuestionEditComponent is not part of any NgModule or the module has not been imported into your module.
    QuestionEditComponent,
    AnswerListComponent,
    AnswerEditComponent,
    ResultListComponent,
    ResultEditComponent
  ],
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    HttpClientModule,
    FormsModule,
    RouterModule.forRoot([
      { path: '', redirectTo: 'home', pathMatch: 'full' },
      { path: 'home', component: HomeComponent },
      { path: 'quiz/create', component: QuizEditComponent },
      { path: 'quiz/edit/:id', component: QuizEditComponent },
      { path: 'quiz/:id', component: QuizComponent },
      { path: 'question/edit/:id', component: QuestionEditComponent },
      { path: 'question/create/:id', component: QuestionEditComponent },
      { path: 'answer/edit/:id', component: AnswerEditComponent },
      { path: 'answer/create/:id', component: AnswerEditComponent },
      { path: 'result/edit/:id', component: ResultEditComponent },
      { path: 'result/create/:id', component: ResultEditComponent },
      { path: 'about', component: AboutComponent },
      { path: 'login', component: LoginComponent },
      { path: '**', component: PageNotFoundComponent }
    ])
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
