import 'package:flutter/material.dart';
import 'package:flutter_application/shared/controllers/post_controller.dart';
import 'package:flutter_application/shared/models/post_model.dart';

class EditPostModal extends StatelessWidget {
  String id;
  EditPostModal(this.id, {super.key});
  late Post post;
  PostController postController = PostController();
  TextEditingController titleEditController = TextEditingController();
  TextEditingController contentEditController = TextEditingController();
  @override
  Widget build(BuildContext context) {
    return Scaffold();
  }
}
