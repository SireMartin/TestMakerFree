import { Component, Inject, Input, OnInit } from "@angular/core";
import { HttpClient } from "@angular/common/http";

@Component({
  selector: "quiz-list",
  templateUrl: './quiz-list.component.html',
  styleUrls: ['./quiz-list.component.css']
})

export class QuizListComponent implements OnInit{
  @Input() class: string;
  title: string;
  selectedQuiz: Quiz;
  quizzes: Quiz[];

  constructor(private http: HttpClient,
    @Inject('BASE_URL') private baseUrl: string) { //private keyword maakt hier private class variabelen  van (p130)
    this.title = "Latest Quizzes";
    this.baseUrl = baseUrl;
    this.http = http;
  }

  ngOnInit() {
    console.log("QuizListComponent instantiated with the following class : " + this.class);
    var url = this.baseUrl + "api/quiz/";
    switch (this.class) {
      case "byTitle":
        url += "ByTitle/";
        break;
      case "random":
        url += "Random/";
        break;
      case "latest":
      default:
        url += "latest/";
    }
    this.http.get<Quiz[]>(url).subscribe(result => {
      this.quizzes = result;
    }, error => console.error(error));
  }

  onSelect(quiz: Quiz) {
    this.selectedQuiz = quiz;
    console.log("quiz with Id " + this.selectedQuiz.Id + " has been selected.")
  }
}
