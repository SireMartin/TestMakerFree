import { Inject, Input, Component, OnChanges, SimpleChanges } from "@angular/core";
import { Router } from "@angular/router";
import { HttpClient } from "@angular/common/http";

@Component({
  selector: "answer-list",
  templateUrl: "./answer-list.component.html",
  styleUrls: ["./answer-list.component.css"]
})

export class AnswerListComponent implements OnChanges{
  @Input() question: Question;
  title: string;
  answers: Answer[];

  constructor(private http: HttpClient,
    @Inject("BASE_URL") private baseUrl: string,
    private router: Router) {
    this.answers = [];
  }

  //SimpleChanges is een hashtabel met als key de naam van de property
  /*!!!!! let op !!!!
   * bij de class de implements niet vergeten
   * wanneer verkeerde functie gedeclareerd wordt, geen foutmeldingen en debugger komt niet in functie (bv onNgChanges(changes: SimpleChanges)) */    
  ngOnChanges(changes: SimpleChanges) {
    if (typeof changes["question"] !== "undefined") {
      var sc = changes["question"];
      if (!sc.isFirstChange()) {
        this.loadData();
      }
    }
  }

  loadData() {
    var url = this.baseUrl + "api/answer/All/" + this.question.Id;
    this.http.get<Answer[]>(url).subscribe(res => {
      this.answers = res;
      console.log("all answers retrieved in anwer-list.component for question ID " + this.question.Id);
      this.title = "Answers for question " + this.question.Text + ".";
    }, error => console.log(error));
  }

  onEdit(answer: Answer) {
    this.router.navigate(["answer/edit", answer.Id]);
  }

  onCreate(answer: Answer) {
    this.router.navigate(["answer/create", this.question.Id]);
  }

  onDelete(answer: Answer) {
    var url = this.baseUrl + "answer/delete/" + answer.Id;
    this.http.delete(url).subscribe(res => {
      console.log("Answer with ID " + answer.Id + " has been deleted");
      //refresh the question list
      this.loadData();
    }, error => console.log(error));
  }
}
