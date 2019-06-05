import { Component, Inject, OnInit } from "@angular/core";
import { HttpClient } from "@angular/common/http";
import { Router, ActivatedRoute } from "@angular/router";
import { error } from "protractor";

@Component({
  selector: "answer-edit",
  templateUrl: "./answer-edit.component.html",
  styleUrls: ["./answer-edit.component.css"]
})

export class AnswerEditComponent {
  title: string;
  answer: Answer;
  //this item will be TRUE when editing an existing answer, FALSE when creating a new one
  editMode: boolean;

  constructor(private activatedRoute: ActivatedRoute,
    private router: Router,
    private http: HttpClient,
    @Inject('BASE_URL') private base_url: string) {
    //create an empty object from the Question interface
    this.answer = <Answer>{};

    //a plus before a variable returns its number representation!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
    var id = +this.activatedRoute.snapshot.params["id"];

    //check if we're in edit-mode or not
    this.editMode = (this.activatedRoute.snapshot.url[1].path === "edit");
    if (this.editMode) {
      //fetch the answer from the server: in this case the id is the answerId
      var url = this.base_url + "api/answer/" + id;
      this.http.get<Answer>(url).subscribe(res => {
        this.answer = res;
        this.title = "Edit - " + this.answer.Text;
      }, error => console.log(error))
    }
    else {
      //create new answer for the Question: in this case the id is the questionId
      this.answer.QuestionId = id;
      this.title = "Create a new Answer";
    }
  }

  onSubmit(answer: Answer) {
    var url = this.base_url + "api/answer";
    if (this.editMode) {
      this.http.post<Answer>(url, answer).subscribe(res => {
        var v = res;
        console.log("Answer " + v.Id + " has been updated.");
        this.router.navigate(["question/edit", v.QuestionId]);
      }, error => console.log(error));
    }
    else {
      this.http.put<Answer>(url, answer).subscribe(res => {
        var v = res;
        console.log("Answer " + v.Id + " has been created");
        this.router.navigate(["question/edit", v.QuestionId]);
      }, error => console.log(error));
    }
  }

  onBack() {
    this.router.navigate(["question/edit", this.answer.QuestionId]);
  }

}
