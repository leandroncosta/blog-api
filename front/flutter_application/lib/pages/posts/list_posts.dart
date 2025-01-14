import 'package:flutter/material.dart';
import 'package:flutter_application/components/card_post.dart';
import 'package:flutter_application/shared/models/post_model.dart';

class ListPosts extends StatelessWidget {
  final List<Post> posts;

  const ListPosts({super.key, required this.posts});

  @override
  Widget build(BuildContext context) {
    return ListView.builder(
      padding: const EdgeInsets.only(bottom: 50),
      itemCount: posts.length,
      itemBuilder: ((context, index) {
        final post = posts[index];
        return CardPost(
          post: post,
          index: index,
        );
      }),
    );
  }
}
